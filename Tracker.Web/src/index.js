import React from 'react';
import ReactDOM from 'react-dom';
import ApolloClient from "apollo-boost";
import { ApolloProvider } from "react-apollo";
import gql from "graphql-tag";

import registerServiceWorker from './registerServiceWorker';
import App from './App/App';

import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import '../node_modules/font-awesome/css/font-awesome.min.css';
import '../node_modules/toastr/build/toastr.min.css';
import './assets/css/main.css';

const client = new ApolloClient({
    uri: "https://w5xlvm3vzz.lp.gql.zone/graphql"
  });

// import { loadIssueTrackers } from './_actions/issueTrackerActions';
// import { setupLookups } from './_actions/lookupActions';

client
  .query({
    query: gql`
      {
        rates(currency: "USD") {
          currency
        }
      }
    `
  })
  .then(result => console.log(result));

ReactDOM.render(

    <ApolloProvider  client={client}>
        <App />
    </ApolloProvider >,

    document.getElementById('root'));

registerServiceWorker();
