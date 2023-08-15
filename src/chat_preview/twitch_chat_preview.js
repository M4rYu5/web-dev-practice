import { MessageUpdater } from "./message_updater/messageUpdater.js";

let twitchStreamer = "xqc"

let messageUpdater = new MessageUpdater(twitchStreamer);


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