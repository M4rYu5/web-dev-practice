
export function encodeString(string) {
    return string.replace(/[\u00A0-\u9999<>\&]/gim, function (i) {
        return '&#' + i.charCodeAt(0) + ';';
    });
}

export function createElementFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    return div.firstChild;
}

export function createMultipleElementsFromHTML(htmlString) {
    var div = document.createElement('div');
    div.innerHTML = htmlString.trim();

    return div.childNodes;
}

export class CustomEvent {

    #callbacks = []

    constructor() {}

    trigger(args){
        this.#callbacks.forEach(f => {
            f[1].apply(f[0], args)
        });
    }

    /** inset callback (only one per function) */
    add(object, callback){
        let c = [object, callback];
        if(this.#callbacks.includes(c))
            return;
        this.#callbacks.push(c);
    }

    remove(object, callback){
        let c = [object, callback];
        if(this.#callbacks.includes(c))
            return;
        this.#callbacks.pop(c);
    }

}
