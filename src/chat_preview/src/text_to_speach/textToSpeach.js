import { MessageDTO } from "../message_updater/messageDTO.js";
import { MessageSpeakOptions } from "./messageSpeakOptions.js";

export class TextToSpeach {

    #isAvailable
    #ttsVoices = {}; // dictionary
    #defaultSpeakOptions;

    #fieldset = document.getElementById("tts-fieldset");
    #content = document.getElementById("tts-content");

    get isActive(){
        try{
            return Object.setPrototypeOf(JSON.parse(localStorage.ttsIsActive), Boolean);
        }
        catch{
            localStorage.ttsIsActive = JSON.stringify(false);
            return false;
        }
    }

    set isActive(value){
        localStorage.ttsIsActive = JSON.stringify(value);
    }


    constructor() {
        this.#isAvailable = window.speechSynthesis != null;
        try{
            const parsedTts = JSON.parse(localStorage.ttsSpeakOptions);
            // keeping the defaults when new property is added in MessageSpeakOptions
            const ttsWithDefautls = {...(new MessageSpeakOptions()), ...parsedTts}
            // setting object type
            this.#defaultSpeakOptions = Object.setPrototypeOf(ttsWithDefautls, MessageSpeakOptions.prototype);
        }
        catch{
            localStorage.clear("ttsSpeakOptions");
            this.#defaultSpeakOptions = new MessageSpeakOptions();
            localStorage.ttsSpeakOptions = JSON.stringify(this.#defaultSpeakOptions)
        }

        this.#setTtsDefaults(this.isActive, this.#defaultSpeakOptions);
        this.#setTtsOptionEvents();
        this.#setContentGrowHandler();
        this.#fillLanguages();
    }


    isAvailable = () => {
        return this.#isAvailable;
    }

    reset = () => {
        window.speechSynthesis.cancel();
    }

    speak = (messageDTO, messageSpeakOptions = null) => {
        messageSpeakOptions ??= this.#defaultSpeakOptions;

        if (!this.#isAvailable) {
            return;
        }
        if (!this.isActive) {
            return;
        }
        if (!(messageDTO instanceof MessageDTO)) {
            return;
        }
        if (!(messageSpeakOptions instanceof MessageSpeakOptions)) {
            return;
        }

        const noPerks = !(messageDTO.subscriber || messageDTO.mod || messageDTO.turbo);
        const approvedLength = messageSpeakOptions.maxMessageLength <= 0 
                    || messageDTO.message.length <= messageSpeakOptions.maxMessageLength;
        let canSpeak = (noPerks && messageSpeakOptions.viewers
            || messageDTO.subscriber && messageSpeakOptions.subscribers
            || messageDTO.mod && messageSpeakOptions.mods
            || messageDTO.turbo && messageSpeakOptions.turbo)
            && approvedLength;
        if (!canSpeak) {
            return;
        }

        let message = new SpeechSynthesisUtterance(this.#messageComposer(messageDTO, messageSpeakOptions));
        message.rate = messageSpeakOptions.speed;
        message.volume = messageSpeakOptions.volume;
        if (messageSpeakOptions.languageName != null) {
            message.voice = this.#ttsVoices[messageSpeakOptions.languageName];
        }
        window.speechSynthesis.speak(message);
    }

    #messageComposer = (messageDTO, messageSpeakOptions) => {
        let message = "";
        if (messageSpeakOptions.includeDisplayName) {
            message += messageDTO.userDisplayName + ": ";
        }
        message += messageDTO.message;
        return message;
    }



    #setContentGrowHandler = () => {
        document.getElementById("tts-arrow").onclick = () => {
            if (this.#fieldset.classList.contains("tts-closed")) {
                this.#content.style.maxHeight = this.#content.scrollHeight + "px";
                this.#fieldset.classList.remove("tts-closed");
                document.getElementById("tts-arrow").textContent = "▲"
            } else {
                this.#content.style.maxHeight = "0px";
                this.#fieldset.classList.add("tts-closed");
                document.getElementById("tts-arrow").textContent = "▼"
            }
        }
    }

    #setTtsDefaults = (isActive, currentSpeakOptions) => {
        // set the object's values
        document.getElementById("tts-active").checked = isActive;
        document.getElementById("tts-viewer-toggle").checked = currentSpeakOptions.viewers;
        document.getElementById("tts-sub-toggle").checked = currentSpeakOptions.subscribers;
        document.getElementById("tts-mod-toggle").checked = currentSpeakOptions.mods;
        document.getElementById("tts-turbo-toggle").checked = currentSpeakOptions.turbo;
        document.getElementById("tts-name-toggle").checked = currentSpeakOptions.includeDisplayName;
        document.getElementById("tts-speed-toggle").value = currentSpeakOptions.speed;
        document.getElementById("tts-max-length").value = currentSpeakOptions.maxMessageLength;
        document.getElementById("tts-volume").value = currentSpeakOptions.volume;
    }

    #setTtsOptionEvents = () => {
        // events to change the tts state
        document.getElementById("tts-active").addEventListener("input", (ev) => {
            this.isActive = document.getElementById("tts-active").checked;
            this.reset();
        });
        document.getElementById("tts-viewer-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.viewers = document.getElementById("tts-viewer-toggle").checked;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-sub-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.subscribers = document.getElementById("tts-sub-toggle").checked;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-mod-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.mods = document.getElementById("tts-mod-toggle").checked;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-turbo-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.turbo = document.getElementById("tts-turbo-toggle").checked;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-name-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.includeDisplayName = document.getElementById("tts-name-toggle").checked;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-speed-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.speed = document.getElementById("tts-speed-toggle").value;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-max-length").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.maxMessageLength = document.getElementById("tts-max-length").value;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-voice-select").addEventListener("change", (ev) => {
            let element = document.getElementById("tts-voice-select");
            let name = element.options[element.selectedIndex].getAttribute("data-name");
            this.#defaultSpeakOptions.languageName = name;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
        document.getElementById("tts-volume").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.volume = document.getElementById("tts-volume").value;
            this.#saveDefaultSpeakOptionsInLocalStorage();
        });
    }

    #saveDefaultSpeakOptionsInLocalStorage = () =>{
        localStorage.ttsSpeakOptions = JSON.stringify(this.#defaultSpeakOptions);
        this.reset();
    }
    
    #fillLanguages = () => {
        if (!this.isAvailable()) {
            return;
        }

        // voices are loaded async of page load, and they might not be available at this moment
        const voices = speechSynthesis.getVoices();
        if (voices.length == 0) {
            window.speechSynthesis.onvoiceschanged = () => {
                this.#fillLanguages();
            };
            return;
        }

        this.#ttsVoices = voices.map((x) => {
            let obj = new Object();
            obj[x.name] = x;
            return obj;
        });
        voices.forEach(voice => {
            const option = document.createElement("option");
            option.textContent = `${voice.name} (${voice.lang})`;
            if (voice.default) { option.textContent += " — DEFAULT";}

            this.#ttsVoices[voice.name] = voice;
            option.setAttribute("data-name", voice.name);
            if (voice.name == this.#defaultSpeakOptions.languageName)
                option.selected = true;
            document.getElementById("tts-voice-select").appendChild(option);
        });
    }
}