import React, { Component } from 'react';
import Moment from 'react-moment';
import { userService } from '../services';

export class Users extends Component {
    static displayName = "Users";

    constructor(props) {
        super(props);
        this.state = { users: [], loading: true };
    }

    componentDidMount() {
        this.loadUsers();
    }

    static renderUsersTable(users) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Created</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user =>
                        <tr key={user.id}>
                            <td>{user.id}</td>
                            <td>{user.name}</td>
                            <td><Moment format="DD.MM.YYYY HH:mm:ss">{user.created}</Moment></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Users.renderUsersTable(this.state.users);

        return (
            <div>
                <h1 id="tabelLabel" >Users list</h1>
                {contents}
            </div>
        );
    }

    async loadUsers() {
        const users = await userService.getAll();
        console.log('users', users);
        this.setState({ users: users, loading: false });
    }
}
