


//#region create connection
let connection = new signalR.hubConnectionBuilder()
    .withAutomaticReconnect()
    .withUser('/hubs/online-users')
    .build();

//#endregion

//#region get response hub



//#endregion

//#region start connection

function SuccessConnection()
{

    console.log("successfully connected.");
}

function ErrorConnection()
{
    console.log("error on connected.");
}


connection.start().then(SuccessConnection, ErrorConnection);


//#endregion

//#region other js



//#endregion