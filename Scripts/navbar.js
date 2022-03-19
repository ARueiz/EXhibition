function NavDropDown() {
    const dropList = document.querySelector('.navDrop')
    let list =  dropList.classList ;
    if(list.contains('displayItem')){
        dropList.classList.remove('displayItem')
    }else{
        dropList.classList.add('displayItem')
    }
    // if (dropList.style.display === 'block') {
    //     dropList.style.display = 'none'
    // } else if (dropList.style.display === 'inline-block') {
    //     dropList.style.display = 'none'
    // } else {
    //     dropList.style.display = 'block'
    // }
}