import React, { Component } from "react";
import IssueTrackerTable from "../../Components/issue-tracker/issue-tracker-table";

class IssueTracker extends Component {
  constructor(props, context) {
    super(props, context);

    this.redirectToAddIssueTrackerPage = this.redirectToAddIssueTrackerPage.bind(this);
    this.handleDelete = this.handleDelete.bind(this);
    this.handleEdit = this.handleEdit.bind(this);
    this.reloadTracker = this.reloadTracker.bind(this);
    this.handleNavView = this.handleNavView.bind(this);
  }

  componentDidMount() {
    window.scrollTo(0, 0);
  }

  redirectToAddIssueTrackerPage() {
    this.props.history.push("/tracker");
  }

  reloadTracker() {
    this.props.actions.loadIssueTrackers();
  }

  handleDelete(val) {
    this.props.actions
      .deleteIssueTracker(val)
      .then
      //() => this.redirect()
      //this.props.actions.loadIssueTrackers()
      ()
      .catch(error => {
        //toastr.error(error);
        //this.setState({ saving: false });
      });
  }

  handleEdit(val) {
    this.props.history.push("/issue/" + val.id);
  }

  handleNavView(val) {
    this.props.history.push("/tracker/" + val.id);
  }

  render() {
    return (
      <div>
        <input
          type="submit"
          value="Add Issue Tracker"
          className="btn btn-primary"
          onClick={this.redirectToAddIssueTrackerPage}
        />

        <input
          type="submit"
          value="Reload"
          className="btn btn-primary"
          onClick={this.reloadTracker}
        />

        <IssueTrackerTable
          issueTrackers={this.props.issueTrackers}
          handleDelete={this.handleDelete}
          handleEdit={this.handleEdit}
          handleNavView={this.handleNavView}
        />
      </div>
    );
  }
}


export default IssueTracker; 
