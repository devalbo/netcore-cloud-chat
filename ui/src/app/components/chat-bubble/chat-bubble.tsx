import React from 'react';
import { stringToColor } from '../../utils';
import './chat-bubble.scss';

export interface ChatBubbleProps {
    name: string;
    content: string;
    appearContinued: boolean;
    sentByUser: boolean;
}

export const ChatBubble: React.FunctionComponent<ChatBubbleProps> = ({ 
    name,
    content,
    appearContinued,
    sentByUser,
}) => {
    return (
        <article className={`chat-bubble ${appearContinued ? 'continued' : ''} ${sentByUser ? 'from-user' : ''}`}>
            <h1 style={{ color: stringToColor(name.charAt(0)) }} className='chat-bubble__author'>{name}</h1>
            <p className='chat-bubble__message'>{content}</p>
        </article>
    );
}
