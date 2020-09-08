function Candidacy() {
    let self = this

    self.LastName = ko.observable()
    self.FirstName = ko.observable()
    self.Email = ko.observable()
    self.PhoneNumber = ko.observable()
    self.Date = ko.observable()
}

function ViewModel() {
    let self = this

    self.Candidacies = ko.observableArray()

    self.loadCandidacies = function () {
        $.ajax({
            type: 'GET',
            url: '/Recruteur/GetCandidacies',
            success: function (result) {
                if (result) {
                    result.forEach(function (element) {
                        if (element) {
                            var candidacy = new Candidacy()
                            candidacy.LastName(element.LastName)
                            candidacy.FirstName(element.FirstName)
                            candidacy.Email(element.Email)
                            candidacy.PhoneNumber(element.PhoneNumber)
                            candidacy.Date(element.Date)

                            self.Candidacies.push(candidacy)
                        }
                    });
                }
            }
        });  
    }
}

$(function () {
    vm = new ViewModel()
    ko.applyBindings(vm, document.getElementById('recruiter'))

    vm.loadCandidacies()
});
