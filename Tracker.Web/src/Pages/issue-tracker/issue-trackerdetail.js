import React, { Component } from 'react';

class IssueTrackerDetail extends Component {

    constructor(props, context) {
        super(props, context);

        this.state = {
            issueTracker: Object.assign({}, this.props.issueTracker),
            errors: {},
            saving: false
        };
    }

    componentDidMount () {
        window.scrollTo(0, 0)
    }
    
    render() {
        return (
            <div>No details</div>
        );
    }

}

export default IssueTrackerDetail; 
