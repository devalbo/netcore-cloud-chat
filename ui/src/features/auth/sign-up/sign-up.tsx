import './sign-up.scss';
import React, { useState } from 'react';
import { Link, Navigate } from 'react-router-dom';
import { useAppSelector, useAppDispatch } from '../../../app/hooks';
import {
  signupAsync,
  selectAuth,
} from '../authSlice';

interface SignUpProps {
}

export const SignUpPage: React.FunctionComponent<SignUpProps> = () => {
    
    const authState = useAppSelector(selectAuth);
    const dispatch = useAppDispatch();
  
    const [username, setUsername] = useState('ajb');
    const [name, setName] = useState('ajb');

    if (authState.status === 'logged-in') {
      return (
        <Navigate replace to='/chat' />
      );
    }

    const signUp = () => {
        dispatch(signupAsync({
          userName: username,
          screenName: name,
        }));
    };

    return (
        <div id='container'>
            <div>
              {authState.status}
            </div>
            <div className='sign-up'>
                <div className='welcome-card'>
                    <h1>
                        welcome to <span className='app-name'>the chat site</span>
                    </h1>
                    <h2>Create your account or <Link to='/login'>back to login</Link></h2>
                    <label>
                        Username
                        <input type='text' value={username} onChange={(e) => setUsername(e.target.value.toLowerCase())} />
                    </label>
                    <label>
                        Name
                        <input type='text' value={name} onChange={(e) => setName(e.target.value)} />
                    </label>
                    <button onClick={() => signUp()} type='submit' >
                        Submit
                    </button>
                </div>
            </div>
        </div>
    );
};
