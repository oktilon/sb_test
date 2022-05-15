import { Component } from 'react';
import { Route } from 'react-router-dom';
import { authenticationService } from './services';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Users } from './components/Users';
import { NewUser } from './components/NewUser';
import { Login } from './components/Login';

import "bootstrap/dist/css/bootstrap.min.css";
import './custom.css'

export default class App extends Component {
    static displayName = "Test app";

    constructor(props) {
        super(props);

        const user = authenticationService.currentUserValue;
        this.state = {
            currentUser: user
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
        console.log("App render", currentUser);
        var users = currentUser == null ? null : (<Route path='/users'>
            <Users user={currentUser} />
        </Route>);
        var add = currentUser == null ? null : (<Route path='/add'>
            <NewUser />
        </Route>);

        return (
            <Layout user={currentUser}>
                <Route exact path='/'>
                    <Home user={currentUser} />
                </Route>
                <Route path='/login'>
                    <Login />
                </Route>
                {users}
                {add}
            </Layout>
        );
    }
}
