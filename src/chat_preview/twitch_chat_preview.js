import { MessageUpdater } from "./message_updater/messageUpdater.js";
import { TextToSpeach } from "./text_to_speach/textToSpeach.js";
import { MessageSpeakOptions } from "./text_to_speach/messageSpeakOptions.js";

let twitchStreamer = "xqc"

let messageUpdater = new MessageUpdater(twitchStreamer);
let tts = new TextToSpeach();
messageUpdater.onMessageReceived.add(null, (message) => tts.speak(message));



