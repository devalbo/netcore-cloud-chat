// feel free to change all schemas

export interface User {
    id: string;
    name: string
}

export interface Message {
    id: string;
    sentBy: User;
    sentAt: Date;
    content: string;
}

export interface Chatroom {
    id: string;
    mostRecentMessage?: Message;
    name: string;
}
