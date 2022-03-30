import './api-connection.scss';
import React, { useEffect, useState } from 'react';
import { APIService } from '../../services';


export const DemoAPIConnection: React.FunctionComponent = () => {
    const [loaded, setLoaded] = useState<boolean>(false);
    const [errored, setErrored] = useState<boolean>(false);
    const [timeElapsed, setTimeElapsed] = useState<number>(1);

    useEffect(() => {
        if (timeElapsed > 3) {
            fetchData();
        }

        async function fetchData() {
            try {
                await APIService.ping();
                setErrored(false);
            }
            catch (e) {
                setErrored(true);
            }
            setLoaded(true);
            setTimeout(() => {
                setLoaded(false);
                setTimeElapsed(1);
            }, 1000);
        }
    }, [setLoaded, setErrored, timeElapsed]);

    useEffect(() => {
        if (timeElapsed > 3) return;

        const intervalId = setTimeout(() => {
            setTimeElapsed(timeElapsed + 1);
        }, 1000);

        return () => clearTimeout(intervalId);
    }, [timeElapsed]);

    const showApiConnection = () => {
        if (loaded && errored) {
            return 'failed!'
        }
        if (loaded) {
            return 'pong!'
        }
        return <>ping{Array(timeElapsed).fill('').map(() => '.')}</>
    }

    return (
        <div className='api-connection'>
            <h3>
                API Status:
            </h3>
            <p>{showApiConnection()}</p>
        </div>
    );
}
