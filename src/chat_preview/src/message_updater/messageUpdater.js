import { encodeString, createElementFromHTML, CustomEvent } from "../util.js";
import { MessageDTO } from "./messageDTO.js";

/**
 * Manages the chat HTML: connects to twitch, adds new messages to chat and manages the scroll
 */
export class MessageUpdater {

    onMessageReceived = new CustomEvent();

    #messageContainer = document.getElementById("messages");
    #shoudScrollToLastMessage = true;

    #twitchStreamer;
    #tmiClient;
    #maxMessages;
    #autoscrollClampHeight;

    /**
     * Creates a new MessageUpdater to handle the chat messages
     * @param {String} twitchStreamer twitch streamer name
     * @param {int} maxMessages number of messages keept on chat, older messages will be deleted
     * @param {int} autoscrollClampHeight scroll more than this number and the auto-scroll to last message will stop
     */
    constructor(twitchStreamer, maxMessages = 300, autoscrollClampHeight = 100) {
        this.#twitchStreamer = twitchStreamer;
        this.#maxMessages = maxMessages;
        this.#autoscrollClampHeight = autoscrollClampHeight;
        this.#updateConnection();
    }


    /**
     * Connect to a new twitch chat
     * @param {String} twitchStreamer twitch streamer name
     */
    changeStreamer = (twitchStreamer) => {
        this.#twitchStreamer = twitchStreamer;
        this.#updateConnection();
        this.clearMesages();
    }

    /**
     * disconnects from chat
     */
    disconnect = () => {
        this.#tmiClient.reconnect = false;
        this.#tmiClient.disconnect();
    }

    /**
     * clear chat
     */
    clearMesages = () => {
        this.#messageContainer.innerHTML = "";
    }


    /** Creates or updates the connection */
    #updateConnection = () =>{
        if (!(this.#tmiClient == null))
            this.disconnect();
        this.#tmiClient = this.#makeTwitchTmiClient(this.#twitchStreamer);
        this.#setCallbacks(this.#tmiClient);
        this.#messageContainer.addEventListener("scroll", this.#messageScrolled);
        this.#setLinks();
    }

    /** tmi.js constructor */
    #makeTwitchTmiClient = (streamer) => {
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

    /** set callbacks on tmi.js client */
    #setCallbacks = (client) => {
        client.connect().catch(console.error);
        client.on('message', this.#onMessageReceived);
    }

    /** handle the received message from tmi.js client */
    #onMessageReceived = (channel, tags, message, self) => {
        let messageDTO = this.#constructMessageDTO(channel, tags, message, self);

        let chatMessage = this.#createChatMessage(messageDTO.userDisplayName, messageDTO.message, messageDTO.color);
        this.#messageContainer.appendChild(chatMessage)

        while (this.#messageContainer.childElementCount > this.#maxMessages) {
            this.#messageContainer.firstChild.remove();
        }

        if (this.#shoudScrollToLastMessage) {
            this.#messageContainer.scrollTop = this.#messageContainer.scrollHeight - this.#messageContainer.clientHeight;
        }

        this.onMessageReceived.trigger([messageDTO]);
    }

    /** creates the HTML message element */
    #createChatMessage = (user, message, color) => {
        let html = '<div class="message-line"> \
                        <span class="chatter" style="color: ' + color + '">' + encodeString(user) + '</span> \
                        <span class="chatter-message">' + encodeString(message) + '</span> \
                    </div>';
        let element = createElementFromHTML(html);
        return element;
    }

    /** handle the autoscroll */
    #messageScrolled = () => {
        let scrollAmount = this.#messageContainer.scrollHeight - this.#messageContainer.clientHeight - this.#messageContainer.scrollTop;
        if (scrollAmount > this.#autoscrollClampHeight && this.#shoudScrollToLastMessage) {
            this.#shoudScrollToLastMessage = false;
        }
        if (scrollAmount <= this.#autoscrollClampHeight && !this.#shoudScrollToLastMessage) {
            this.#shoudScrollToLastMessage = true;
        }
    }

    /** set the chat text and links to the stream */
    #setLinks = () => {
        let textLink = document.getElementById("title-text-link");
        textLink.href = "https://twitch.tv/" + this.#twitchStreamer;
        textLink.textContent = "twitch.tv/" + this.#twitchStreamer;
    }


    #constructMessageDTO = (channel, tags, message, self) => {
        return new MessageDTO(
            channel.substring(1),
            tags["username"],
            self, 
            tags["display-name"],
            message,
            tags["color"],
            tags["mod"],
            tags["subscriber"],
            tags["turbo"])
    }

}