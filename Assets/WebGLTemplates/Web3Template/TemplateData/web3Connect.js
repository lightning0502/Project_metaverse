// initializer

/*
    <script type="module" src="https://cdn.jsdelivr.net/npm/web3@latest/dist/web3.min.js"></script>
    <script type="module" src="https://cdn.jsdelivr.net/npm/node-fetch@3.1.1/src/index.min.js"></script>
*/

// https://github.com/w3c/autoplay/issues/1, https://github.com/WebAudio/web-audio-api/issues/1759
const audioContext = new AudioContext();
if (audioContext.state === 'suspended') {
    if (document.autoplayPolicy === 'allowed') {
        // State is suspended only because hardware is slow.
    }

    if (document.autoplayPolicy === 'disallowed' ||
        document.autoplayPolicy === 'allowed-muted') {
        // State is suspended because of the autoplay policy.
        // TODO: Call audioContext.resume() after user gesture.
    }
}
console.error("audioContext.state : " + audioContext.state);

if (window.ethereum) {
    web3 = new Web3(window.ethereum);
    console.error("web3 : ", web3);
    console.error("utils : ", web3.utils);
    console.error("givenProvider : ", web3.givenProvider);
    console.error("providers : ", web3.providers);
    console.error("modules : ", web3.modules); // undefined

    window.ethereum.send('eth_requestAccounts').then(async (accounts) => {
        window.accounts = await accounts;
        console.log("window.accounts : ", window.accounts);
    });

    /*
    window.ethereum.enable();
    try { // connect popup
        // window.ethereum.enable().then(function () {
        window.ethereum.on("accountsChanged", function () {
            location.reload();
            console.error("location : ", location);

            // const accounts = await web3.eth.getAccounts();
        });

    } catch (error) {
        if (error.code === 4001) {
            // User rejected request
            alert("로그인을 해주세요!");
        }

        else
            alert(error.code);
    }
    */

    /*
    async () => {
        const balance = await web3.eth.getBalance(accounts[0]);
        console.log("balance", web3.utils.fromWei(balance, "ether"));
    };
    */
}

else
    alert("Metamask 확장 프로그램을 설치해주세요!");