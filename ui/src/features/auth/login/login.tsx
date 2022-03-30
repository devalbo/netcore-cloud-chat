import './login.scss';
import React, { useState } from 'react';
import { Link, Navigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { loginAsync, selectAuth } from '../authSlice';
import { TokenService } from '../../../services/token.service';

export interface LoginProps { }

export const LoginPage: React.FunctionComponent<LoginProps> = () => {
    const authState = useAppSelector(selectAuth);
    const dispatch = useAppDispatch();

    const [username, setUsername] = useState('ajb');

    if (authState.status === 'logged-in') {
      return (
        <Navigate replace to='/chat' />
      );
    }

    if (TokenService.hasAuthToken()) {
      dispatch(loginAsync({
        userName: username
      }));
    }
    
    const login = () => {
      dispatch(loginAsync({
        userName: username
      }));
    }

    return (
        <div id='container'>
            <div>
                {authState.status}
            </div>
            <div className='login'>
                <div className='welcome-card'>
                    <h1>
                        welcome to <span className='app-name'>the chat site</span>
                    </h1>
                    <h2>Please login or <Link to='/sign-up'>sign up</Link></h2>
                    <label>
                        Username
                        <input type='text' value={username} onChange={(e) => setUsername(e.target.value.toLowerCase())} />
                    </label>
                    <button type='submit' onClick={() => login()}>
                        Submit
                    </button>
                </div>
            </div>
        </div>
    );
};
