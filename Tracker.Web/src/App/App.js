import React, { Component } from "react";
import { Router, Route, Switch } from "react-router-dom";
import { history } from "../_helpers/history";

import Authorized from "../Components/hoc/authorization";
import About from "../Pages/anonymous/About";
import Header from "../Components/common/Header";
import Footer from "../Components/common/Footer";
import Dashboard from "../Components/dashboard/Dashboard";
import NoMatch from "../Components/common/NoMatch";

import LoginPage from "../Pages/login/LoginPage";
import NewIssue from "../Pages/issue-tracker/issue-create";
import IssueTracker from "../Pages/issue-tracker/issue-tracker";
import IssueTrackerDetail from "../Pages/issue-tracker/issue-trackerdetail";

class App extends Component {
  constructor(props, context) {
    super(props, context);

    var ls = localStorage.getItem("wss.auth");
    var jwtToken = JSON.parse(ls);
    var auth = false;

    if (jwtToken) auth = true;

    this.state = {
      authenticated: auth
    };

    this.toggleAuthentication  =this.toggleAuthentication.bind(this);
    this.handleLogout  = this.handleLogout.bind(this);
  }

  toggleAuthentication() {
    var ls = localStorage.getItem("wss.auth");
    var jwtToken = JSON.parse(ls);
    var auth = false;

    if (jwtToken) auth = true;

    this.setState({
      authenticated: auth
    });
  }

  handleLogout() {
    this.setState({ authenticated: false });
    this.props.actions.logout();
  }

  render() {
    const User = Authorized(["user", "manager", "admin"]);
    //const Manager = Authorized(["manager", "admin"]);
    //const Admin = Authorized(["admin"]);

    const contentWrapper = {
      padding: 0
    };

    return (
      <Router history={history} {...this.state}>
        <div>
          <Header  authProps={this.state.authenticated} onHandleLogoutClick = {this.handleLogout} />

          <main className="content-wrapper" style={contentWrapper}>
            <div className="container">
              <Switch>
                <Route path="/" exact component={Dashboard} />
                <Route path="/about" component={About} />

                <Switch>
                  <Route path="/issue/new" component={NewIssue} />
                  <Route path="/issue/:id" exact component={NewIssue} />
                  <Route path="/trackers" component={User(IssueTracker)} />
                  <Route path="/tracker" exact component={IssueTrackerDetail} />
                  <Route path="/tracker/:id" component={IssueTrackerDetail} />
                  <Route path="/login" render={(routeProps) => (<LoginPage changeLoginAuth={this.toggleAuthentication} {...routeProps} {...this.props} {...this.state}/>)} />
                  <Route component={NoMatch} />
                </Switch>

                <Route component={NoMatch} />
              </Switch>
            </div>
          </main>

          <Footer />
        </div>
      </Router>
    );
  }
}

export default App;
