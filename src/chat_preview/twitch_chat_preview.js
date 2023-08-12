import { encodeString } from "./util.js";


const twitchStreamer = "xqc";
const messageContainer = document.getElementById("messages");
const tmiClient = makeTwitchTmiClient(twitchStreamer);


let maxMessages = 300;
let shoudScrollToLastMessage = true;
let autoscrollClampHeight = 100;


setCallbacks(tmiClient)
messageContainer.addEventListener("scroll", messageScrolled);



function makeTwitchTmiClient(streamer) {
    let client = new tmi.Client({
        options: { debug: false },
        identity: {
            // username: 'bot_name',
            // password: 'oauth:my_bot_token'
        },
        channels: [streamer]
    });
    return client
}


function setCallbacks(client) {
    client.connect().catch(console.error);
    client.on('message', onMessageReceived);
}


function onMessageReceived(channel, tags, message, self) {
    //	if(self) return;
    let chatMessage = createChatMessage(tags["display-name"], message, tags["color"]);
    messageContainer.appendChild(chatMessage)

    while (messageContainer.childElementCount > maxMessages) {
        messageContainer.firstChild.remove();
    }

    if (shoudScrollToLastMessage){
        messageContainer.lastChild.scrollIntoView();
    }
}


function createChatMessage(user, message, color) {
    let html = '<div class="message-line"> \
                    <span class="chatter" style="color: ' + color + '">' + encodeString(user) + '</span> \
                    <span class="chatter-message">' + encodeString(message) + '</span> \
                </div>';
    let element = createElementFromHTML(html);
    return element;
}


function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    // Change this to div.childNodes to support multiple top-level nodes.
    return div.firstChild;
}



// auto scroll
function messageScrolled(){
    let scrollAmount = messageContainer.scrollHeight - messageContainer.clientHeight - messageContainer.scrollTop;
    if (scrollAmount > autoscrollClampHeight && shoudScrollToLastMessage){
        shoudScrollToLastMessage = false;
    }
    if (scrollAmount <= autoscrollClampHeight && !shoudScrollToLastMessage){
        shoudScrollToLastMessage = true;
    }
}
