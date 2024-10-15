/* p5 Animation in Starter Page */
document.addEventListener('mousemove', (e) => {
    const pupils = [document.getElementById('pupilX'), document.getElementById('pupilO')];
    const eyeCenters = pupils.map(pupil => {
        if (pupil) {
            const rect = pupil.getBoundingClientRect();
            return { x: rect.left + rect.width / 2, y: rect.top + rect.height / 2 };
        }
        return { x: 0, y: 0 };
    });

    pupils.forEach((pupil, index) => {
        if (pupil) {
            const dx = e.clientX - eyeCenters[index].x;
            const dy = e.clientY - eyeCenters[index].y;
            const angle = Math.atan2(dy, dx);
            const distance = Math.min(Math.hypot(dx, dy), 20); 
            const pupilX = Math.cos(angle) * distance;
            const pupilY = Math.sin(angle) * distance;

            pupil.style.transform = `translate(${pupilX}px, ${pupilY}px)`;
        }
    });
});
