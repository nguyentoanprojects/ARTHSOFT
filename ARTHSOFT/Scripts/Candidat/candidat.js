function Element() {
    let self = this

    self.Value = ko.observable()
}

function Contact() {
    let self = this

    self.LastName = ko.observable()
    self.FirstName = ko.observable()
    self.Email = ko.observable()
    self.PhoneNumber = ko.observable()
    self.Address = ko.observable()
    self.City = ko.observable()
    self.ZIP = ko.observable()
}

function Candidacy() {
    let self = this

    self.Contact = ko.observable(new Contact())
    self.Formations = ko.observableArray()
    self.Experiences = ko.observableArray()
    self.Competences = ko.observable()
    self.Skills = ko.observable()
    self.OtherInformation = ko.observable()

    self.addFormation = () => self.Formations.push(new Element())
    self.deleteFormation = (element) => self.Formations.remove(element)

    self.addExperience = () => self.Experiences.push(new Element())
    self.deleteExperience = (element) => self.Experiences.remove(element)
}

function ViewModel() {
    let self = this

    self.Candidacy = ko.observable(new Candidacy())
    self.Loading = ko.observable(false)

    self.Apply = async function () {
        if (self.Candidacy().Contact().FirstName() && self.Candidacy().Contact().LastName()
            && self.Candidacy().Contact().Email() && self.Candidacy().Contact().PhoneNumber()
            && self.Candidacy().Skills()
            && (self.Candidacy().Experiences().length > 0)
            && (self.Candidacy().Formations().length > 0)) {

            self.Loading(true)
           
            const resultVerifyContact = await self.VerifyContact()
            if (!resultVerifyContact.Status) {
                self.showModalDialog('ERREUR', resultVerifyContact.Error)
            }
            else {
                var formData = new FormData()
                formData.append('Candidacy', ko.toJSON(vm.Candidacy()))

                var fileImage = $('#uploadImage').get(0).files
                if (fileImage.length > 0)
                    formData.append('UploadImage', fileImage[0])

                var fileCV = $('#uploadCV').get(0).files
                if (fileCV.length > 0)
                    formData.append('UploadCV', fileCV[0])

                $.ajax({
                    type: 'POST',
                    url: '/Candidat/Apply',
                    contentType: 'application/json',
                    data: formData,
                    processData: false,
                    contentType: false,
                    error: function () {
                        self.Loading(false)
                        self.showModalDialog('ERREUR', 'Une erreur est survenue lors de la soumission de votre candidature')
                    },
                    success: function (result) {
                        self.Loading(false)

                        if (!result.Status) {
                            self.showModalDialog('ERREUR', resultVerifyContact.Error)
                        }
                        else {
                            self.showModalDialog(
                                'CANDIDATURE ENVOYEE',
                                'Merci d\'avoir soumise votre candidature, nous reviendrons vers vous très prochainement. \n Vous allez être redirigé automatiquement vers la page d\'accueil'
                            )

                            $('#closeModal').hide();
                            setTimeout(() => document.location.href = '/', 5000)
                        }

                    }
                });
            }
        }
        else {
            self.showModalDialog('ATTENTION', 'Veuillez remplir tous les champs obligatoires *')
        }
    }

    self.VerifyContact = async function () {
        const result = await $.ajax({
            type: 'GET',
            url: '/Candidat/VerifyContact?email=' + self.Candidacy().Contact().Email(),
            error: () => self.Loading(false),
            success: function (result) {
                if (!result.Status) {
                    self.Loading(false)
                    self.showModalDialog('ERREUR', result.Error)
                }
            }
        });

        return result;
    }

    self.showModalDialog = function (title, message, redirection) {
        const modalDialog = $('#modalDialog')

        modalDialog.find('#title').html(title)
        modalDialog.find('#error').html(message)

        modalDialog.modal("show");
    }
}

$(function () {
    vm = new ViewModel();
    ko.applyBindings(vm, document.getElementById('candidate'));

    let autocomplete = new google.maps.places.Autocomplete($("#inputAddress")[0], {});
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        let place = autocomplete.getPlace()
        console.log(place.address_components)
    });

    $("#modalDialog").on("hidden.bs.modal", () => console.log('redirect'))
});
