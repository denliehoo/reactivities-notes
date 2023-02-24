import React from "react";
import ReactDOM from "react-dom/client";
import "semantic-ui-css/semantic.min.css";

import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  //<React.StrictMode>
  <App />
  //</React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

/* 
  React.StrictMode causes our App to render twice to check stuff.
  This is only for in development. 
  In production, it will only render once. 
  Can turn of if have certain costly APIs
*/
