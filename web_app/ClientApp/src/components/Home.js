import React, { Component } from 'react';

export class Home extends Component {
    static displayName = 'Home';

    render() {
        const { user } = this.props;
        const name = user == null ? 'Incognito' : user.name;
        return (
            <div>
                <h1>Hello, {name}!</h1>
            </div>
        );
    }
}
