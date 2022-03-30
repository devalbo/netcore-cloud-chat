import React from 'react';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import './App.css';
import { DemoAPIConnection } from './features/_api-connection/api-connection';
import { LoginPage } from './features/auth/login/login';
import { SignUpPage } from './features/auth/sign-up/sign-up';
import { ChatPage } from './features/chat/chat';


export const App: React.FunctionComponent = () => {
  return (
      <React.StrictMode>
          <DemoAPIConnection />
          <BrowserRouter>
              <Routes>
                  <Route path='/login' element={<LoginPage/>} />
                  <Route path='/sign-up' element={<SignUpPage/>} />
                  <Route path='/chat/*' element={<ChatPage/>} />
                  <Route
                      path='*'
                      element={<Navigate to='/login' />}
                  />
              </Routes>
          </BrowserRouter>
      </React.StrictMode>);
}




export default App;
