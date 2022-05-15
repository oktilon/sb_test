import { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import PropTypes from 'prop-types';

export class Layout extends Component {
    static displayName = 'Layout';

    render() {
        return (
            <div>
                <NavMenu user={this.props.user} />
                <Container>
                    {this.props.children}
                </Container>
            </div>
        );
    }
};

Layout.propTypes = {
    user: PropTypes.object,
    children: PropTypes.object
}