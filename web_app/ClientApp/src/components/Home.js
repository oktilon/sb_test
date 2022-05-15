import { Component } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';

export class Home extends Component {
    static displayName = 'Home';

    getUserText() {
        return (
            <p>You can manage users <Link to="/users">here</Link></p>
        );
    }

    getIncognitoText() {
        return (
            <p>You have to <Link to="/login">log in</Link> to continue</p>
        );
    }

    render() {
        const { user } = this.props;
        const name = user == null ? 'Incognito' : user.name;
        const text = user == null ? this.getIncognitoText() : this.getUserText();
        return (
            <div>
                <h1>Hello, {name}!</h1>
                {text}
            </div>
        );
    }
};

Home.propTypes = {
    user: PropTypes.object
};
