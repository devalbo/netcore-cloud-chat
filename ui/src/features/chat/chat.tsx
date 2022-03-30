import './chat.scss';
import { Link, Navigate, NavLink, Route, Routes } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import { stringToColor } from '../../app/utils';
import { Chatroom, ChatroomCard, NewChatroom } from '../../app/components';
import { models } from '../../app/models';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { logoutAsync, selectAuth } from '../auth/authSlice';
import { getAllRoomsAsync, selectRoom } from './room/roomSlice';

export interface ChatProps { }

export const ChatPage: React.FunctionComponent<ChatProps> = () => {

    const authState = useAppSelector(selectAuth);
    const roomState = useAppSelector(selectRoom);
    const dispatch = useAppDispatch();

    const [getAllRoomsCalled, setGetAllRoomsCalled] = useState(false);

    useEffect(() => {
      if (authState.status === 'logged-in' && !getAllRoomsCalled) {
        dispatch(getAllRoomsAsync());
        setGetAllRoomsCalled(true);
      }
    }, [authState.status, dispatch, getAllRoomsCalled]);
    
    const currentUser: models.User | undefined = authState.currentUser;
    const chatrooms = roomState.chatRooms;

    const logout = () => {
      dispatch(logoutAsync());
    };

    const refreshRooms = () => {
      dispatch(getAllRoomsAsync());
    }

    const sortChatrooms = (_1: object, _2: object) => {
        return 0;
    };

    if (authState.status !== 'logged-in') {
      return (
        <Navigate replace to='/login' />
      );
    }

    return (
        <div id='container'>
            <div className='chatroom'>
                <aside className='chatroom__sidebar'>
                    <section className='sidebar__header'>
                        <div className='identifiers'>
                            <h1>the chat site</h1>
                            <h2>
                                chatting as <span className='name' style={{ color: stringToColor(currentUser?.name) }}>{currentUser?.name}</span>
                            </h2>
                        </div>
                        <div className='actions'>
                            <button onClick={() => logout()}>logout</button>
                            <button onClick={() => refreshRooms()}>refresh rooms</button>
                        </div>
                    </section>
                    <ul className='sidebar__list'>
                        <div className='new-room-option'>
                            <Link to={'new'}>create new room</Link>
                        </div>
                        {chatrooms
                            ?.slice()
                            ?.sort((c1, c2) => sortChatrooms(c1, c2))
                            ?.map(c => (
                                <li key={c.id}>
                                    <NavLink
                                        className={isActive => isActive ? 'is-active' : ''}
                                        to={`${c.id}`}>
                                        <ChatroomCard mostRecentMessage={c.mostRecentMessage} name={c.name} />
                                    </NavLink>
                                </li>
                            ))}
                    </ul>
                </aside>
                <main className='chatroom__window'>
                    <Routes>
                        <Route path='' element={
                            <div className='no-chatroom'>
                                <h3>Join a chatroom...</h3>
                            </div>
                        }>
                        </Route>
                        <Route path='new' element={<NewChatroom/>}>
                        </Route>
                        <Route path=':id' element={<Chatroom/>} />
                    </Routes>
                </main>
            </div>
        </div >
    );
};
