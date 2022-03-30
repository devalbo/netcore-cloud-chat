import React from 'react';
import { models } from '../../models';
import { stringToColor, timestampToTimeAgo } from '../../utils';
import './chatroom-card.scss';

export interface ChatroomCardProps {
    name: string;
    mostRecentMessage?: models.Message;
}

export const ChatroomCard: React.FunctionComponent<ChatroomCardProps> = ({ name, mostRecentMessage }) => {
    const firstLetter = name.charAt(0).toUpperCase();

    return (
        <article className='chatroom__card'>
            <span className='card__icon' style={{ backgroundColor: stringToColor(firstLetter) }}>
                {firstLetter}
            </span>
            <div className='card__info'>
                <h1>{name}</h1>
                {mostRecentMessage ? (
                    <p>{mostRecentMessage.sentBy.name.split(' ')[0]}: {mostRecentMessage.content}</p>
                ) : (
                    <p>No messages yet</p>
                )}
            </div>
            <div className='card__timestamp'>
                {mostRecentMessage && (
                    <span>{timestampToTimeAgo(mostRecentMessage.sentAt)}</span>
                )}
            </div>
        </article>
    );
}
