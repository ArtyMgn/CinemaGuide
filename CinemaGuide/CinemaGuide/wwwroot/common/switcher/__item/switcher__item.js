function switchSource(source) {
    const searchLists = document.getElementsByClassName('search-list');

    for (let searchList of searchLists) {
        const searchListSource = searchList.attributes.getNamedItem('source').value;
        searchList.style.display = searchListSource === source ? 'block' : 'none';
    }
}