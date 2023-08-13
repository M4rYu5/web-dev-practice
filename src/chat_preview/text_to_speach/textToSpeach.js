import { MessageDTO } from "../message_updater/messageDTO.js";
import { MessageSpeakOptions } from "./messageSpeakOptions.js";

export class TextToSpeach {
    #isAvailable

    constructor(){
        this.#isAvailable = window.speechSynthesis != null;
    }

    isAvailable = () => {
        return this.#isAvailable;
    }

    reset = () =>{
        window.speechSynthesis.cancel();
    }

    speak = (messageDTO, messageSpeakOptions) => {
        if (!this.#isAvailable){
            return
        }
        if (!(messageDTO instanceof MessageDTO)) {
            return;
        }
        if (!(messageSpeakOptions instanceof MessageSpeakOptions)) {
            return;
        }
        let canSpeak = (messageSpeakOptions.all
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
}