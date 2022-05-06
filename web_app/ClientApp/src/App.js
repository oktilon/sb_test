import React, { Component } from 'react';
import { Route } from 'react-router';
import { authenticationService } from './services';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Users } from './components/Users';

import './custom.css'

export default class App extends Component {
    static displayName = "Test app";

    constructor(props) {
        super(props);

        this.state = {
            currentUser: null
        }
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(x => this.setState({ currentUser: x }));
    }

    logout() {
        authenticationService.logout();
    }

    render() {
        const { currentUser } = this.state;

        return (
            <Layout user={currentUser}>
                <Route exact path='/'>
                    <Home />
                </Route>
                <Route path='/users'>
                    <Users user={currentUser} />
                </Route>
            </Layout>
        );
    }
}
