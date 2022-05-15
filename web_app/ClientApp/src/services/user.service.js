import { authHeader, handleResponse } from '../helpers';

export const userService = {
    getAll,
    addUser
};

function getAll() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`user`, requestOptions).then(handleResponse);
}

function addUser(name) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name })
    };

    return fetch(`user`, requestOptions)
        .then(handleResponse)
        .then(ok => {
            return ok;
        });
}