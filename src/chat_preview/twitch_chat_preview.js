import { MessageUpdater } from "./message_updater/messageUpdater.js";
import { TextToSpeach } from "./text_to_speach/textToSpeach.js";
import { MessageSpeakOptions } from "./text_to_speach/messageSpeakOptions.js";

let twitchStreamer = "xqc"

let messageUpdater = new MessageUpdater(twitchStreamer);
let tts = new TextToSpeach();
messageUpdater.onMessageReceived.add(null, (message) => tts.speak(message));


// tts content 
var fieldset = document.getElementById("tts-fieldset");
var content = document.getElementById("tts-content");

document.getElementById("tts-arrow").onclick = function () {
  if (fieldset.classList.contains("tts-closed")) {
    content.style.maxHeight = content.scrollHeight + "px";
    fieldset.classList.remove("tts-closed");
  } else {
    content.style.maxHeight = "0px";
    fieldset.classList.add("tts-closed");
  }
}