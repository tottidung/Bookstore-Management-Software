const listImgs = document.querySelector('.list-images');
const imgs = document.getElementsByTagName('img');
const length = imgs.length;
const btnLeft = document.querySelector('.btn-left');
const btnRight = document.querySelector('.btn-right');
let current = 0;
const handleChangeSlide =
    () => {
        if (current == length - 1) {
            current = 0;
            let width = imgs[0].offsetWidth
            listImgs.style.transform = `translateX(0px)`
            document.querySelector('.active').classList.remove('active')
            document.querySelector('index-item-' + current).classList.add('active')
        }
        else {
            current++;
            let width = imgs[0].offsetWidth
            listImgs.style.transform = `translateX(${width * -1 * current}px)`
            document.querySelector('.active').classList.remove('active')
            document.querySelector('index-item-' + current).classList.add('active')
        }
    }


let handleEventChangeSlide = setInterval(handleChangeSlide, 4000)

btnRight.addEventListener('click', () => {
    clearInterval(handleEventChangeSlide)
    handleChangeSlide()
    handleEventChangeSlide = setInterval(handleChangeSlide, 4000)
})

btnLeft.addEventListener('click', () => {
    if (current == 0) {
        current = length - 1;
        let width = imgs[0].offsetWidth
        listImgs.style.transform = `translateX(${width * -1 * current}px)`
    }
    else {
        current--;
        let width = imgs[0].offsetWidth
        listImgs.style.transform = `translateX(${width * -1 * current}px)`
    }
    handleEventChangeSide = setInterval(handleChangeSlide, 4000)
})