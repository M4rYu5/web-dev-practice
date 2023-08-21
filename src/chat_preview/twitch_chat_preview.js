import { MessageUpdater } from "./message_updater/messageUpdater.js";
import { TextToSpeach } from "./text_to_speach/textToSpeach.js";

let twitchStreamer = "xqc"

let messageUpdater = new MessageUpdater(twitchStreamer);
let tts = new TextToSpeach();
messageUpdater.onMessageReceived.add(null, (message) => tts.speak(message));



document.getElementById("title-gear").onclick = (ev) =>{
    document.getElementById("title-text-link").classList.toggle("display-none");
    document.getElementById("streamer-form").classList.toggle("display-none");
}


