import { authenticationService } from '../services';

export function handleResponse(response) {
    return response.text().then(text => {
        const data = text && (text.substr(0, 1) === "{" || text.substr(0, 1) === "[") ? JSON.parse(text) : {};
        if (!response.ok) {
            if ([401, 403].indexOf(response.status) !== -1) {
                authenticationService.logout();
            }

            const error = (data && data.message) || text || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}