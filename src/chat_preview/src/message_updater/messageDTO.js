/** Keep message related data in a single place */
export class MessageDTO{
    channel;
    username;
    isSelf;
    userDisplayName;
    message;
    color;
    mod;
    subscriber;
    turbo;

    /**
     * Construct a MessageDTO object that keeps all message related data.
     * @param {String} channel chat channel / streamer
     * @param {String} user user that send the message (not same as Display Name)
     * @param {boolean} isSelf message was sent by the library connected user
     * @param {String} userDisplayName user display name (use this to display the user name)
     * @param {String} message the message sent by the user
     * @param {String} color the color of the user (display) name
     * @param {boolean} mod is mod in current channel?
     * @param {boolean} subscriber is subscriber in current channel?
     * @param {boolean} turbo has turbo?
     */
    constructor(channel, username, isSelf, userDisplayName, message, color, mod, subscriber, turbo){
        this.channel = channel;
        this.username = username;
        this.isSelf = isSelf;
        this.userDisplayName = userDisplayName;
        this.message = message;
        this.color = color;
        this.mod = mod;
        this.subscriber = subscriber;
        this.turbo = turbo;
    }
}