import { MessageUpdater } from "./message_updater/messageUpdater.js";
import { TextToSpeach } from "./text_to_speach/textToSpeach.js";


const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const streamerUrlParam = urlParams.get("streamer");

if(streamerUrlParam != null){
    document.forms["streamer-form"].elements["streamer"].value = streamerUrlParam;
}

let twitchStreamer = streamerUrlParam ?? "xqc"

let messageUpdater = new MessageUpdater(twitchStreamer);
let tts = new TextToSpeach();
messageUpdater.onMessageReceived.add(null, (message) => tts.speak(message));



document.getElementById("title-gear").onclick = (ev) =>{
    document.getElementById("title-text-link").classList.toggle("display-none");
    document.getElementById("streamer-form").classList.toggle("display-none");
}


