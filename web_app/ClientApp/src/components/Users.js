import { Component } from 'react';
import Moment from 'react-moment';
import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import { userService } from '../services';
import { Link } from 'react-router-dom';

export class Users extends Component {
    static displayName = "Users";

    constructor(props) {
        super(props);
        this.addNew = this.addNew.bind(this);
        this.state = { users: [], loading: true };
    }

    addNew() {
        window.location.href = '/add';
    }

    componentDidMount() {
        console.log("Users did mount", this.props.user);
        if (this.props.user != null) {
            this.loadUsers();
        }
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
        if (this.props.user == null) {
            return (
                <div>
                    <p>You have to <Link to="/login">log in</Link> to access this page</p>
                </div>
            );
        }

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Users.renderUsersTable(this.state.users);

        return (
            <div>
                <h1 id="tabelLabel" >Users list
                    <Button className='float-right' color='primary' onClick={this.addNew}>
                        New user
                    </Button>
                </h1>
                {contents}
            </div>
        );
    }

    async loadUsers() {
        const users = await userService.getAll();
        console.log('users', users);
        this.setState({ users: users, loading: false });
    }
};

Users.propTypes = {
    user: PropTypes.object
};
