// WebGL initializer

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

if (window.ethereum) {
    // initialize
    window.accounts = undefined;
    web3 = new Web3(window.ethereum);
    console.error("web3 : ", web3);
    console.error("utils : ", web3.utils);
    console.error("givenProvider : ", web3.givenProvider);
    console.error("providers : ", web3.providers);
    console.error("modules : ", web3.modules); // undefined

    alert("연동하실 계정을 하나만 선택해주세요. 중복으로 선택해도 1개만 활성화됩니다." + '\n' + "Please select only one account to link. Even if you select duplicate, only one will be active.");

    // request wallet account with metamask
    window.ethereum.request({ method: "eth_requestAccounts" }).then(async (accounts) => {
        // 여러개의 계정을 선택해도 한 개만 연동됨.
        window.accounts = accounts[0];

        const balance = await web3.eth.getBalance(accounts[0]);
        console.log("current balance : ", web3.utils.fromWei(balance, "ether"));

    }).catch((error) => {
        alert("error code : " + error.code + " / " + error.message + " page will be refreshed.");

        if (error.code === 4001) { // reject from user
            setTimeout(function () {
                window.location.reload();
            }, 5000);
        }
    });
}

else
    alert("Metamask 확장 프로그램을 설치해주세요!");