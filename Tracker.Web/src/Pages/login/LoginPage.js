import React, { Component } from "react";
import toastr from "toastr";

class LoginPage extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      username: "",
      password: "",
      submitted: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }


  componentDidMount() {
    window.scrollTo(0, 0);
  }

  handleChange(e) {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  }

  handleSubmit(e) {
    e.preventDefault();

    this.setState({ submitted: true });
    const { username, password } = this.state;

    if (username && password) {
      this.props.actions
        .login(username, password)
        .then(() => this.redirect())
        .catch(error => {
          toastr.error(error);
          this.setState({ saving: false });
        });
    }
  }

  redirect() {
    this.props.changeLoginAuth();
    this.setState({ saving: false });
    toastr.success("Login Successful.");
    this.props.history.push("/trackers");
  }

  render() {
    const loginCard = {
      margin: "40px 0 0 0"
    };

    const { username, password } = this.state;
    return (
      <div>
        <section className="card-section">
          <div className="container">
            <div className="col-lg-8 col-lg-offset-2">
              <div className="card text-center login-box" style={loginCard}>
                <header className="text-center">
                  <h2 className="section-title">SignIn Now</h2>
         
                </header>
                <div className="icon">
                  <img src="assets/images/icon/icon-login.png" alt="" />
                </div>

                <form name="form" onSubmit={this.handleSubmit}>
                  <div className="row">
                    <div className="col-md-6">
                      <input
                        type="text"
                        name="username"
                        value={username}
                        onChange={this.handleChange}
                        className="search-field"
                        placeholder="Username"
                      />
                    </div>
                    <div className="col-md-6">
                      <input
                        type="text"
                        name="password"
                        value={password}
                        onChange={this.handleChange}
                        className="search-field"
                        placeholder="Password"
                      />
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-12">
                      <input
                        type="submit"
                        className="btn btn-success"
                        value="Let me enter"
                      />
                      <p className="forget-pass">
                        Have you forgot your username or password ?{" "}
                      </p>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </section>
      </div>
    );
  }
}

export default LoginPage;
