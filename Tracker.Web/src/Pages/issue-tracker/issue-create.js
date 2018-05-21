import React, { Component } from 'react';
import toastr from 'toastr';
import NewIssueForm from '../../Components/issue-tracker/new-issue-form';

class IssueTrackerCreate extends Component {

    constructor(props, context) {
        super(props, context);

        this.state = {
            issueTracker: Object.assign({}, this.props.issueTracker),
            errors: {},
            saving: false
        };

        this.saveIssueTracker = this.saveIssueTracker.bind(this);
        this.updateIssueTrackerState = this.updateIssueTrackerState.bind(this);
        this.redirectToAddIssueTrackerPage = this.redirectToAddIssueTrackerPage.bind(this);
        this.reloadTracker = this.reloadTracker.bind(this);
        this.handleNavView = this.handleNavView.bind(this);
    }
    
    componentDidMount () {
        window.scrollTo(0, 0)
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.issueTracker.id !== nextProps.issueTracker.id) {
            // Necessary to populate form when existing course is loaded directly.
            this.setState({ issueTracker: Object.assign({}, nextProps.issueTracker) });
        }
    }

    redirectToAddIssueTrackerPage() {
        this.props.history.push('/tracker');
    }

    reloadTracker() {
        this.props.actions.loadIssueTrackers();
    }

    handleNavView(val) {
        this.props.history.push('/tracker/' + val.id);
    }

    updateIssueTrackerState(event) {
        const field = event.target.name;
        let issueTracker = Object.assign({}, this.state.issueTracker);
        issueTracker[field] = event.target.value;
        return this.setState({ issueTracker: issueTracker });
    }

    saveIssueTracker(event) {
        event.preventDefault();

        this.setState({ saving: true });
        this.props.actions.saveIssueTracker(this.state.issueTracker)
            .then(() => this.redirect())
            .catch(error => {
                toastr.error(error);
                this.setState({ saving: false });
            });
    }

    redirect() {
        this.setState({saving: false});
        toastr.success('Tracker saved.');
        this.props.history.push('/trackers');
      }
    

    render() {
        return (
            <div>
               <NewIssueForm 
                    issueTracker={this.state.issueTracker}
                    onChange={this.updateIssueTrackerState}
                    departments={this.props.departments}
                    issueTypes={this.props.issueTypes}
                    onSave={this.saveIssueTracker}
                    errors={this.state.errors}
                    saving={this.state.saving} />
            </div>
        );
    }
}

export default IssueTrackerCreate;