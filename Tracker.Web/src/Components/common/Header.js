import React, { Component } from "react";
import { NavLink } from "react-router-dom";
import PropTypes from "prop-types";
import Logo from "../../assets/images/logo.png";
import "../../Components/common/custom-component.css";

class Header extends Component {
  onHandleLogoutClick() {
    this.props.onHandleLogoutClick();
  }

  render() {
    return (
      <div>
        <header className="hero overlay">
          <nav className="navbar">
            <div className="container">
              <div className="navbar-header">
                <button
                  type="button"
                  className="navbar-toggle collapsed"
                  data-toggle="collapse"
                  data-target="#navbar-collapse"
                  aria-expanded="false"
                >
                  <span className="sr-only">Toggle navigation</span>
                  <span className="fa fa-bars" />
                </button>
                <NavLink to="/" className="brand">
                  <img src={Logo} alt="Knowledge" />
                </NavLink>
              </div>
              <div className="navbar-collapse collapse" id="navbar-collapse">
                <ul className="nav navbar-nav navbar-right">
                  <li>
                    <NavLink to="/">Home</NavLink>
                  </li>
                  <li>
                    <NavLink to="/trackers">Browse</NavLink>
                  </li>

                  <li>
                    {!this.props.authProps && (
                      <NavLink to="/login">Login</NavLink>
                    )}
                    {this.props.authProps && (
                      <a onClick={this.onHandleLogoutClick.bind(this)}>
                        Logout
                      </a>
                    )}
                  </li>

                  <li>
                    <NavLink
                      to="/issue/new"
                      className="btn btn-success nav-btn"
                    >
                      New Issue
                    </NavLink>
                  </li>
                </ul>
              </div>
            </div>
          </nav>
        </header>
      </div>
    );
  }
}

Header.contextTypes = {
  router: PropTypes.object
};

// function mapStateToProps(state, ownProps) {
//   return {
//     authenticated: state.authenticated
//   };
// }
export default Header; //connect(mapStateToProps, userActions)(Header);
