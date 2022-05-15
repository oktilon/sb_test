import { Component } from "react";
import Form from "react-validation/build/form";
import Input from "react-validation/build/input";
import CheckButton from "react-validation/build/button";
import { userService } from '../services';


const required = value => {
  if (!value) {
    return (
      <div className="alert alert-danger" role="alert">
        This field is required!
      </div>
    );
  }
};

export class NewUser extends Component {

  constructor(props) {
    super(props);
    this.handleSave = this.handleSave.bind(this);
    this.onChangeName = this.onChangeName.bind(this);
    this.state = {
      name: "",
      saving: false,
      message: ""
    };
  }

  onChangeName(e) {
    this.setState({
      name: e.target.value
    });
  }

  handleSave(e) {
    e.preventDefault();
    this.setState({
      message: "",
      saving: true
    });
    this.form.validateAll();
    if (this.checkBtn.context._errors.length === 0) {
      userService.addUser(this.state.name).then(
        () => {
          window.location.href = "/users";
        },
        error => {
          const resMessage =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString();
          this.setState({
            saving: false,
            message: resMessage
          });
        }
      );
    } else {
      this.setState({
        saving: false
      });
    }
  }
  render() {
    return (
      <div className="row">
        <div className="col-sm-0 col-md-3 col-lg-4"></div>
        <div className="col-sm-12 col-md-6 col-lg-4">
          <div className="card card-container">
            <img
              src="/user_add.png"
              alt="profile-img"
              className="profile-img-card"
            />
            <Form
              onSubmit={this.handleSave}
              ref={c => {
                this.form = c;
              }}
            >
              <div className="form-group">
                <label htmlFor="name">Name</label>
                <Input
                  type="text"
                  className="form-control"
                  name="name"
                  value={this.state.name}
                  onChange={this.onChangeName}
                  validations={[required]}
                />
              </div>
              <div className="form-group">
                <button
                  className="btn btn-primary btn-block"
                  disabled={this.state.saving}
                >
                  {this.state.saving && (
                    <span className="spinner-border spinner-border-sm"></span>
                  )}
                  <span>Add user</span>
                </button>
              </div>
              {this.state.message && (
                <div className="form-group">
                  <div className="alert alert-danger" role="alert">
                    {this.state.message}
                  </div>
                </div>
              )}
              <CheckButton
                style={{ display: "none" }}
                ref={c => {
                  this.checkBtn = c;
                }}
              />
            </Form>
          </div>
        </div>
        <div className="col-sm-0 col-md-3 col-lg-4"></div>
      </div>
    );
  }
}
