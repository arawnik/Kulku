using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Models;
using Kulku.Domain;
using Kulku.Domain.Contacts;
using Kulku.Domain.Network;
using Kulku.Web.Admin.Components.Inbox.Components;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Inbox.Pages;

partial class ContactRequests
{
    [Inject]
    private IQueryHandler<
        GetContactRequests.Query,
        IReadOnlyList<ContactRequestModel>
    > ContactRequestQueries { get; set; } = null!;

    [Inject]
    private ICommandHandler<UpdateContactRequestStatus.Command> UpdateStatusHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<ConvertContactRequest.Command> ConvertHandler { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkCompanies.Query,
        IReadOnlyList<NetworkCompanyModel>
    > NetworkCompanyQueries { get; set; } = null!;

    [Inject]
    private ICommandHandler<CreateCompany.Command, Guid> CreateCompanyHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<EnrollNetworkCompany.Command, Guid> EnrollCompanyHandler { get; set; } =
        null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

    private IReadOnlyList<ContactRequestModel> _allRequests = [];
    private IReadOnlyList<NetworkCompanyModel> _enrolledCompanies = [];
    private ContactRequestStatus? _statusFilter = ContactRequestStatus.New;
    private string? _errorMessage;

    private ContactRequestModel? _convertRequest;
    private ConvertContactRequestModal? _convertModal;

    protected override async Task OnInitializedAsync()
    {
        await LoadRequests();
    }

    private IReadOnlyList<ContactRequestModel> FilteredRequests =>
        _statusFilter.HasValue
            ? [.. _allRequests.Where(r => r.Status == _statusFilter.Value)]
            : _allRequests;

    private void SetFilter(ContactRequestStatus? status)
    {
        _statusFilter = status;
    }

    private async Task LoadRequests()
    {
        var result = await ContactRequestQueries.Handle(
            new GetContactRequests.Query(),
            CancellationToken
        );
        _allRequests = result.IsSuccess ? result.Value ?? [] : [];
    }

    private async Task HandleMarkSpam(Guid id)
    {
        var result = await UpdateStatusHandler.Handle(
            new UpdateContactRequestStatus.Command(id, ContactRequestStatus.Spam),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            _errorMessage = null;
            await LoadRequests();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to mark as spam.";
        }
    }

    private async Task HandleDismiss(Guid id)
    {
        var result = await UpdateStatusHandler.Handle(
            new UpdateContactRequestStatus.Command(id, ContactRequestStatus.Dismissed),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            _errorMessage = null;
            await LoadRequests();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to dismiss.";
        }
    }

    private async Task HandleConvert(Guid id)
    {
        var request = _allRequests.FirstOrDefault(r => r.Id == id);
        if (request is null)
            return;

        // Load enrolled companies for the picker
        var companiesResult = await NetworkCompanyQueries.Handle(
            new GetNetworkCompanies.Query(LanguageContext.Current),
            CancellationToken
        );
        _enrolledCompanies = companiesResult.IsSuccess ? companiesResult.Value ?? [] : [];

        _convertRequest = request;
        _convertModal?.Load(request);
    }

    private async Task HandleConvertSave(ConvertContactRequestModal.ConvertFormModel form)
    {
        if (_convertRequest is null)
            return;

        Guid companyId;

        if (form.IsNewCompany)
        {
            // Create the company first
            var translations = LanguageCodeMapper
                .SupportedLanguageCodes.Select(lang => new CompanyTranslationDto(
                    lang,
                    form.NewCompanyName,
                    string.Empty
                ))
                .ToList();

            var createResult = await CreateCompanyHandler.Handle(
                new CreateCompany.Command(
                    form.NewCompanyWebsite,
                    form.NewCompanyRegion,
                    translations
                ),
                CancellationToken
            );

            if (!createResult.IsSuccess)
            {
                _convertModal?.SetError(createResult.Error?.Message ?? "Failed to create company.");
                return;
            }

            companyId = createResult.Value;

            // Enroll in the network
            var enrollResult = await EnrollCompanyHandler.Handle(
                new EnrollNetworkCompany.Command(companyId, CompanyStage.Watchlist, null, []),
                CancellationToken
            );

            if (!enrollResult.IsSuccess)
            {
                _convertModal?.SetError(
                    enrollResult.Error?.Message ?? "Failed to enroll company in network."
                );
                return;
            }
        }
        else
        {
            companyId = form.SelectedCompanyId;
            if (companyId == Guid.Empty)
            {
                _convertModal?.SetError("Please select a company.");
                return;
            }
        }

        var result = await ConvertHandler.Handle(
            new ConvertContactRequest.Command(
                _convertRequest.Id,
                companyId,
                string.IsNullOrWhiteSpace(form.Summary) ? null : form.Summary.Trim()
            ),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            _errorMessage = null;
            CloseConvertModal();
            await LoadRequests();
        }
        else
        {
            _convertModal?.SetError(result.Error?.Message ?? "Failed to convert contact request.");
        }
    }

    private void CloseConvertModal()
    {
        _convertRequest = null;
    }
}
