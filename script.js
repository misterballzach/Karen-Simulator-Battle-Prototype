document.addEventListener('DOMContentLoaded', () => {
    const characterMouth = document.getElementById('character-mouth');
    const audio = document.getElementById('audio');

    const mouthShapes = [
        'assets/hecomi-a.png',
        'assets/hecomi-i.png',
        'assets/hecomi-u.png',
        'assets/hecomi-e.png',
        'assets/hecomi-o.png',
        'assets/hecomi-n.png'
    ];
    const defaultShape = 'assets/hecomi-base.png';
    const audioFile = 'assets/voice.mp3';

    let animationInterval;

    const setMouthShape = (shape) => {
        characterMouth.src = shape;
    };

    const startAnimation = () => {
        let currentIndex = 0;
        animationInterval = setInterval(() => {
            setMouthShape(mouthShapes[currentIndex]);
            currentIndex = (currentIndex + 1) % mouthShapes.length;
        }, 150); // Change shape every 150ms
    };

    const stopAnimation = () => {
        clearInterval(animationInterval);
        setMouthShape(defaultShape);
    };

    // Initialize
    audio.src = audioFile;
    setMouthShape(defaultShape);

    audio.addEventListener('play', startAnimation);
    audio.addEventListener('pause', stopAnimation);
    audio.addEventListener('ended', stopAnimation);
});
