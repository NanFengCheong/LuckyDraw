import { CountUp } from './countUp.js';

window.loadSignalRfunctions = () => {
    window.connection.on("ReceiveMessage", function (buttonId) {
        if (window.location.href.indexOf('admin') < 0 && window.location.href.indexOf('drawresult') < 0 && window.location.href.indexOf('vip') < 0) {
            if (window.location.href.indexOf('single') < 0 && buttonId.toLowerCase().indexOf('single') > 0) {
                console.log(window.location.href)
                window.location.assign(window.location.origin + "/singleprizedraw");
            }
            else if (window.location.href.indexOf('multi') < 0 && buttonId.toLowerCase().indexOf('multi') > 0) {
                console.log(window.location.href)
                window.location.assign(window.location.origin + "/multiprizedraw");
            }
        }
        clickButtonUsingId(buttonId);
    });
}

window.sendButtonEvent = (msg) => {
    window.connection.invoke("SendMessage", msg).catch(function (err) {
        return console.error(err.toString());
    });
};

window.clickButtonUsingId = (id) => {
    document.getElementById(id).click();
}

window.setSingleWinner = (winnerWwidId, winnerNameId, winnerWwid, winnerName) => {
    window.runDrawAnimation();
    window.setWinnerText(winnerWwidId, winnerNameId, winnerWwid, winnerName);

}

window.setMultiWinner = (winnerArray) => {    
    if (winnerArray) {
        window.runDrawAnimation();
        winnerArray.forEach(winner => {
            window.setWinnerText(winner.winnerWwidId, winner.winnerNameId, winner.winnerWwid, winner.winnerName);
        })
    }
}

var isDrawAnimationRunning = false;

window.setWinnerText = (winnerWwidId, winnerNameId, winnerWwid, winnerName) => {
    var animationDuration = randomIntFromInterval(7000, 12000);
    var winnerEl = document.getElementById(winnerWwidId);
    var winnerNameEl = document.getElementById(winnerNameId);

    setAnimationClass(winnerNameEl, 'text-focus-in')
    winnerNameEl.innerText = 'TBD...';
    
    var countUp = new CountUp(winnerWwidId, winnerWwid,
        {
            startVal: 10000000,
            separator: '',
            duration: animationDuration / 1000,
            easingFn: (t, b, c, d) => {
                var ts = (t /= d) * t;
                var tc = ts * t;
                return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
            }
        });
    isDrawAnimationRunning = true;
    countUp.start(() => {
        setAnimationClass(winnerNameEl, 'tracking-in-expand')
        winnerNameEl.innerText = winnerName;

        if (isDrawAnimationRunning) {
            window.runNormalAnimation();
            isDrawAnimationRunning = false;
        }

        //anime({
        //    targets: winnerNameEl,
        //    opacity: {
        //        value: [0, 1],
        //        duration: 1000,
        //    },
        //});
    });
}

function randomIntFromInterval(min, max) { // min and max included
    return Math.floor(Math.random() * (max - min + 1) + min);
}

window.runNormalAnimation = (arg) => {
    var particles = Particles.init({
        selector: '.background',
        color: ['#1a2980', '#0b9492'],
        maxParticles: 100,
        sizeVariations: 5,
        speed: 0.3,
        minDistance: 150,
        connectParticles: false
    });
}

window.runDrawAnimation = (arg) => {
    var particles = Particles.init({
        selector: '.background',
        color: ['#333333', '#dd1818'],
        maxParticles: 100,
        sizeVariations: 5,
        speed: 0.9,
        minDistance: 150,
        connectParticles: true
    });
}

window.setAnimationClass = (
    htmlElement,
    animationEntranceClass,
    animationEntranceClassRemoveDelay = 3000
) => {
    if (htmlElement) {
        htmlElement.classList.add(animationEntranceClass);
        if (animationEntranceClassRemoveDelay) {
            setTimeout(() => {
                htmlElement.classList.remove(animationEntranceClass)
            }, animationEntranceClassRemoveDelay)
        }
    } 
}