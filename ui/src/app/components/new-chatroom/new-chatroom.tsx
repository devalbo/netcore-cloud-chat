import React, { useState } from 'react';
import { Navigate } from 'react-router-dom';
import { createRoomAsync, selectRoom } from '../../../features/chat/room/roomSlice';
import { useAppDispatch, useAppSelector } from '../../hooks';
import './new-chatroom.scss';

export interface NewChatroomProps {
}

export const NewChatroom: React.FunctionComponent<NewChatroomProps> = () => {
    const dispatch = useAppDispatch();
    const roomState = useAppSelector(selectRoom);

    const [name, setName] = useState('r1');
    const creatingRoom = false;

    const create = () => {
        dispatch(createRoomAsync({name}));
    }

    if (roomState.createdRoomId !== undefined) {
        return (
            <Navigate replace to={`/chat/${roomState.createdRoomId}`} />
        );
    }

    return (
        <div className='new-chatroom'>
            <div className='card'>
                <h2>Choose a name for the new room</h2>
                <label>
                    Room name
                    <input disabled={creatingRoom} value={name} onChange={(e) => setName(e.target.value)} />
                </label>
                <button disabled={creatingRoom} onClick={() => create()}>
                    Create Room
                </button>
            </div>
        </div>
    );
};
