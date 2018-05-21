import React from 'react';
//import { NavLink } from "react-router-dom";

const LoginForm = ({ loading }) => {
    const loginCard = {
        margin: '40px 0 0 0'
      };
      
    return (
        <div>
            <section className="card-section">
                <div className="container">
                    <div className="col-lg-8 col-lg-offset-2">
                        <div className="card text-center login-box" style={loginCard}>

                            <header className="text-center">
                                <h2 className="section-title">SignIn Now</h2>
                                {/*
                                <div className="section-subtitle">Join Our wonderful community and let others help you without a single penny</div>
                                */}
                            </header>
                            <div className="icon">
                                <img src="assets/images/icon/icon-login.png" alt="" />
                            </div>

                            <form>
                                <div className="row">
                                    <div className="col-md-6">
                                        <input type="text" className="search-field" placeholder="Email Address" />
                                    </div>
                                    <div className="col-md-6">
                                        <input type="text" className="search-field" placeholder="Password" />
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <input type="submit" className="btn btn-success" value="Let me enter" />
                                        <p className="forget-pass">Have you forgot your username or password ? </p>
                                    </div>
                                </div>
                            </form>

                        </div>

                    </div>
                </div>
            </section>
        </div>
    );

};

export default LoginForm
