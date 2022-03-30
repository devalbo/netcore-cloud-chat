import './chatroom.scss';
import React, { useCallback, useEffect, useState } from 'react';
import { ChatBubble } from '../chat-bubble/chat-bubble';
import { models } from '../../models';
import { useParams } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { selectAuth } from '../../../features/auth/authSlice';
import { MessageService } from '../../../services/message.service';
import { Message } from '../../models/models';
import { clearCreatedRoomId } from '../../../features/chat/room/roomSlice';

export interface ChatroomProps { }

type RouteParams = {
    id: string;
}

export const Chatroom: React.FunctionComponent<ChatroomProps> = () => {
    const [message, setMessage] = useState('');
    const [messages, setMessages] = useState([] as Message[]);
    const [sendingMessage, setSendingMessage] = useState(false);
    const [loadingMessages, setLoadingMessages] = useState(false);
    const { id } = useParams<RouteParams>();
    const dispatch = useAppDispatch();

    const [loadedMessagesRoomId, setLoadedMessagesRoomId] = useState(undefined as (string | undefined));

    const authState = useAppSelector(selectAuth);
    const currentUser: models.User | undefined = authState.currentUser;

    dispatch(clearCreatedRoomId());

    const sendMessage = () => {
      setSendingMessage(true);
      MessageService.sendMessage(id!, message)
        .then((res) => {
          console.log(res);
          setMessage('');
          refreshMessages();
        })
        .finally(() => {
          setSendingMessage(false);
        });
    };

    const refreshMessages = useCallback(() => {
      setLoadingMessages(true);
      MessageService.getMessagesForRoom(id!)
        .then((res: Message[]) => {
          console.log(res);
          setMessages(res);
        })
        .finally(() => {
          setLoadingMessages(false);
        });
    }, [id]);

    useEffect(() => {
        if (loadedMessagesRoomId !== id) {
          refreshMessages();
          setLoadedMessagesRoomId(id);
        }
    }, [loadedMessagesRoomId, id, refreshMessages]);

    return (
        <section className='chat'>
            <main className='chat__view'>
                {(messages || [])?.map((m, i) => {
                    const isContinued = (i + 1 < messages!.length) && messages![i + 1].sentBy.id === m.sentBy.id;
                    const isCurrentUser = m.sentBy.id === currentUser?.id;
                    return <ChatBubble key={m.id} name={m.sentBy.name} sentByUser={isCurrentUser} appearContinued={isContinued} content={m.content} />
                })}
            </main>
            <footer className='chat__input'>
                <textarea disabled={sendingMessage} value={message} onChange={(e) => setMessage(e.target.value)} />
                <button disabled={sendingMessage} onClick={() => sendMessage()}> Send </button>
            </footer>
        </section>
    )
};
