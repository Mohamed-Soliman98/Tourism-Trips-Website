# Complete ISP/DIP Refactoring Script
# This script continues the refactoring by replacing _unitOfWork.Repository usages
# with injected repository interfaces

Write-Host "Starting ISP/DIP Refactoring..." -ForegroundColor Green

$servicesPath = "Application\Services"
$filesProcessed = 0

# TripTranslations Services
Write-Host "`nProcessing TripTranslations Services..." -ForegroundColor Cyan

# AddTripTranslationService
$file = "$servicesPath\TripTranslations\AddTripTranslationService.cs"
$content = Get-Content $file -Raw
$content = $content -replace 'using Application\.Interfaces\.IUnitOfWork;', "using Application.Interfaces.IUnitOfWork;`nusing Application.Interfaces.Repositories;"
$content = $content -replace 'private readonly IUnitOfWork _unitOfWork;', "private readonly ITripRepository _tripRepository;`n        private readonly ITripTranslationRepository _tripTranslationRepository;`n        private readonly IUnitOfWork _unitOfWork;"
$content = $content -replace 'public AddTripTranslationService\(\s*IUnitOfWork unitOfWork,', "public AddTripTranslationService(`n            IUnitOfWork unitOfWork,`n            ITripRepository tripRepository,`n            ITripTranslationRepository tripTranslationRepository,"
$content = $content -replace '\s*_unitOfWork = unitOfWork;', "            _unitOfWork = unitOfWork;`n            _tripRepository = tripRepository;`n            _tripTranslationRepository = tripTranslationRepository;"
$content = $content -replace '_unitOfWork\.Trips\.GetByIdAsync', '_tripRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.ExistsByTripIdAndLanguageAsync', '_tripTranslationRepository.ExistsByTripIdAndLanguageAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.Add\(', '_tripTranslationRepository.Add('
Set-Content -Path $file -Value $content -NoNewline
Write-Host "  ✓ AddTripTranslationService" -ForegroundColor Green
$filesProcessed++

# DeleteTripTranslationService
$file = "$servicesPath\TripTranslations\DeleteTripTranslationService.cs"
$content = Get-Content $file -Raw
$content = $content -replace 'using Application\.Interfaces\.IUnitOfWork;', "using Application.Interfaces.IUnitOfWork;`nusing Application.Interfaces.Repositories;"
$content = $content -replace 'private readonly IUnitOfWork _unitOfWork;', "private readonly ITripRepository _tripRepository;`n        private readonly ITripTranslationRepository _tripTranslationRepository;`n        private readonly IUnitOfWork _unitOfWork;"
$content = $content -replace 'public DeleteTripTranslationService\(IUnitOfWork unitOfWork\)', "public DeleteTripTranslationService(`n            IUnitOfWork unitOfWork,`n            ITripRepository tripRepository,`n            ITripTranslationRepository tripTranslationRepository)"
$content = $content -replace '\s*_unitOfWork = unitOfWork;', "            _unitOfWork = unitOfWork;`n            _tripRepository = tripRepository;`n            _tripTranslationRepository = tripTranslationRepository;"
$content = $content -replace '_unitOfWork\.Trips\.GetByIdAsync', '_tripRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.GetByIdAsync', '_tripTranslationRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.Remove\(', '_tripTranslationRepository.Remove('
Set-Content -Path $file -Value $content -NoNewline
Write-Host "  ✓ DeleteTripTranslationService" -ForegroundColor Green
$filesProcessed++

# GetTripTranslationByIdService
$file = "$servicesPath\TripTranslations\GetTripTranslationByIdService.cs"
$content = Get-Content $file -Raw
$content = $content -replace 'using Application\.Interfaces\.IUnitOfWork;', "using Application.Interfaces.Repositories;"
$content = $content -replace 'private readonly IUnitOfWork _unitOfWork;', "private readonly ITripTranslationRepository _tripTranslationRepository;"
$content = $content -replace 'public GetTripTranslationByIdService\(IUnitOfWork unitOfWork\)', "public GetTripTranslationByIdService(ITripTranslationRepository tripTranslationRepository)"
$content = $content -replace '\s*_unitOfWork = unitOfWork;', "            _tripTranslationRepository = tripTranslationRepository;"
$content = $content -replace '_unitOfWork\.TripTranslations\.GetByIdAsync', '_tripTranslationRepository.GetByIdAsync'
Set-Content -Path $file -Value $content -NoNewline
Write-Host "  ✓ GetTripTranslationByIdService" -ForegroundColor Green
$filesProcessed++

# GetTripTranslationsService
$file = "$servicesPath\TripTranslations\GetTripTranslationsService.cs"
$content = Get-Content $file -Raw
$content = $content -replace 'using Application\.Interfaces\.IUnitOfWork;', "using Application.Interfaces.Repositories;"
$content = $content -replace 'private readonly IUnitOfWork _unitOfWork;', "private readonly ITripRepository _tripRepository;`n        private readonly ITripTranslationRepository _tripTranslationRepository;"
$content = $content -replace 'public GetTripTranslationsService\(IUnitOfWork unitOfWork\)', "public GetTripTranslationsService(`n            ITripRepository tripRepository,`n            ITripTranslationRepository tripTranslationRepository)"
$content = $content -replace '\s*_unitOfWork = unitOfWork;', "            _tripRepository = tripRepository;`n            _tripTranslationRepository = tripTranslationRepository;"
$content = $content -replace '_unitOfWork\.Trips\.GetByIdAsync', '_tripRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.GetByTripIdAsync', '_tripTranslationRepository.GetByTripIdAsync'
Set-Content -Path $file -Value $content -NoNewline
Write-Host "  ✓ GetTripTranslationsService" -ForegroundColor Green
$filesProcessed++

# UpdateTripTranslationService
$file = "$servicesPath\TripTranslations\UpdateTripTranslationService.cs"
$content = Get-Content $file -Raw
$content = $content -replace 'using Application\.Interfaces\.IUnitOfWork;', "using Application.Interfaces.IUnitOfWork;`nusing Application.Interfaces.Repositories;"
$content = $content -replace 'private readonly IUnitOfWork _unitOfWork;', "private readonly ITripRepository _tripRepository;`n        private readonly ITripTranslationRepository _tripTranslationRepository;`n        private readonly IUnitOfWork _unitOfWork;"
$content = $content -replace 'public UpdateTripTranslationService\(\s*IUnitOfWork unitOfWork,', "public UpdateTripTranslationService(`n            IUnitOfWork unitOfWork,`n            ITripRepository tripRepository,`n            ITripTranslationRepository tripTranslationRepository,"
$content = $content -replace '\s*_unitOfWork = unitOfWork;', "            _unitOfWork = unitOfWork;`n            _tripRepository = tripRepository;`n            _tripTranslationRepository = tripTranslationRepository;"
$content = $content -replace '_unitOfWork\.Trips\.GetByIdAsync', '_tripRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.GetByIdAsync', '_tripTranslationRepository.GetByIdAsync'
$content = $content -replace '_unitOfWork\.TripTranslations\.ExistsByTripIdAndLanguageExcludingIdAsync', '_tripTranslationRepository.ExistsByTripIdAndLanguageExcludingIdAsync'
Set-Content -Path $file -Value $content -NoNewline
Write-Host "  ✓ UpdateTripTranslationService" -ForegroundColor Green
$filesProcessed++

Write-Host "`nCompleted $filesProcessed services" -ForegroundColor Green
Write-Host "Run this script again to continue with remaining services" -ForegroundColor Yellow
Write-Host "`nNext: TripIncludes, TripExcludes, TripFAQs, Categories, Destinations, TourTypes, BookingInquiries, Testimonials, Banners, CMSSections, SiteSettings, Dashboard" -ForegroundColor Yellow
