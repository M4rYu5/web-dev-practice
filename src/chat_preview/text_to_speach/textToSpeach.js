import { MessageDTO } from "../message_updater/messageDTO.js";
import { MessageSpeakOptions } from "./messageSpeakOptions.js";

export class TextToSpeach {
    
    #isAvailable
    #isActive = false;
    #defaultSpeakOptions = new MessageSpeakOptions();

    constructor(){
        this.#isAvailable = window.speechSynthesis != null;

        this.#setTtsDefaults(this.#isActive, this.#defaultSpeakOptions);
        this.#setTtsOptionEvents();
    }

    isAvailable = () => {
        return this.#isAvailable;
    }

    isActive = () => {
        return this.#isActive;
    }

    reset = () =>{
        window.speechSynthesis.cancel();
    }

    speak = (messageDTO, messageSpeakOptions = null) => {
        messageSpeakOptions ??= this.#defaultSpeakOptions;

        if (!this.#isAvailable){
            return;
        }
        if (!this.#isActive){
            return;
        }
        if (!(messageDTO instanceof MessageDTO)) {
            return;
        }
        if (!(messageSpeakOptions instanceof MessageSpeakOptions)) {
            return;
        }

        var noPerks = !(messageDTO.subscriber || messageDTO.mod || messageDTO.turbo);
        let canSpeak = (noPerks && messageSpeakOptions.viewers
            || messageDTO.subscriber && messageSpeakOptions.subscribers
            || messageDTO.mod && messageSpeakOptions.mods
            || messageDTO.turbo && messageSpeakOptions.turbo)
            && messageDTO.message.length <= messageSpeakOptions.maxMessageLength;
        if (!canSpeak) {
            return;
        }

        let message = new SpeechSynthesisUtterance(this.#messageComposer(messageDTO, messageSpeakOptions));
        message.rate = messageSpeakOptions.speed;
        window.speechSynthesis.speak(message);
    }

    #messageComposer = (messageDTO, messageSpeakOptions) => {
        let message = "";
        if(messageSpeakOptions.includeDisplayName){
            message += messageDTO.userDisplayName;
        }
        message += messageDTO.message;
        return message;
    }



    #setTtsDefaults = (isActive, currentSpeakOptions) => {
        // set the object's values
        document.getElementById("tts-arrow").checked = isActive;
        document.getElementById("tts-viewer-toggle").checked = currentSpeakOptions.viewers;
        document.getElementById("tts-sub-toggle").checked = currentSpeakOptions.subscribers;
        document.getElementById("tts-mod-toggle").checked = currentSpeakOptions.mods;
        document.getElementById("tts-turbo-toggle").checked = currentSpeakOptions.turbo;
        document.getElementById("tts-name-toggle").checked = currentSpeakOptions.includeDisplayName;
        document.getElementById("tts-speed-toggle").value = currentSpeakOptions.speed;
        document.getElementById("tts-length-toggle").value = currentSpeakOptions.maxMessageLength;
    }

    #setTtsOptionEvents = () => {
        // events to change the tts state
        document.getElementById("tts-arrow").addEventListener("input", (ev) => {
            this.#isActive = document.getElementById("tts-arrow").checked;
            this.reset();
        });
        document.getElementById("tts-viewer-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.viewers = document.getElementById("tts-viewer-toggle").checked;
            this.reset();
        });
        document.getElementById("tts-sub-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.subscribers = document.getElementById("tts-sub-toggle").checked;
            this.reset();
        });
        document.getElementById("tts-mod-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.mods = document.getElementById("tts-mod-toggle").checked;
            this.reset();
        });
        document.getElementById("tts-turbo-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.turbo = document.getElementById("tts-turbo-toggle").checked;
            this.reset();
        });
        document.getElementById("tts-name-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.includeDisplayName = document.getElementById("tts-name-toggle").checked;
            this.reset();
        });
        document.getElementById("tts-speed-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.speed = document.getElementById("tts-speed-toggle").value;
            this.reset();
        });
        document.getElementById("tts-length-toggle").addEventListener("input", (ev) => {
            this.#defaultSpeakOptions.maxMessageLength = document.getElementById("tts-length-toggle").value;
            this.reset();
        });
    }
}