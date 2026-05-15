using Microsoft.AspNetCore.Components;

namespace SlickSysDev.Public.Components.Pages;

public partial class ServicePricing : ComponentBase
{
  public void OnGet()
  {

  }

  private record PricingPlan(
    string Name,
    string Tagline,
    string Price,
    string AnnualPrice,
    string PriceSuffix,
    List<string> Features,
    string CTA,
    bool Featured);

  private record IncludedItem(string Icon, string Title, string Description);

  private record FaqItem(string Question, string Answer);

  private readonly PricingPlan _plan1 =
    new PricingPlan (
      "Starter",
      "Ideal for small applications and proof-of-concept migrations",
      "$25K",
      "20K",
      "starting from",
      [
        "Free legacy assessment included",
        "Up to 3 applications or services",
        "Single cloud provider (AWS, Azure, or GCP)",
        "Lift-and-shift or re-platform migration",
        "30-day post-migration hypercare",
        "Knowledge transfer sessions",
      ],
      "Get Started",
      false
  );

    private readonly PricingPlan _plan2 = new (
      "Enterprise",
      "For complex, multi-system migrations with compliance requirements",
      "$150K",
      "$140K",
      "starting from",
      [
        "Free legacy assessment included",
        "Unlimited applications and services",
        "Multi-cloud and hybrid cloud support",
        "Full re-architecture and modernization",
        "Security & compliance (SOC 2, HIPAA, PCI-DSS)",
        "90-day post-migration hypercare",
        "Dedicated migration architect",
        "Executive steering committee support",
      ],
      "Talk to Sales",
      true
    );

    private readonly PricingPlan _plan3 = new
    (
      "Managed",
      "Ongoing cloud operations and optimization after migration",
      "$8K",
      "$6K",
      "/ month",
      [
        "24/7 cloud infrastructure monitoring",
        "Incident response and remediation",
        "Monthly cost optimization reviews",
        "Security patching and updates",
        "Quarterly architecture reviews",
        "Dedicated support engineer",
      ],
      "Learn More",
      false
    );
  private readonly List<IncludedItem> _included =
  [
    new("bx bx-search-alt", "Free Assessment",
      "Every engagement starts with a thorough assessment of your current systems at no cost."),
    new("bx bx-shield-quarter", "Security Review",
      "We audit and harden your security posture as part of every migration."),
    new("bx bx-book-open", "Documentation", "Full architecture documentation and runbooks delivered at project close."),
    new("bx bx-group", "Knowledge Transfer",
      "We train your team so they can own and operate the new systems confidently."),
  ];

  private readonly List<FaqItem> _faqs =
  [
    new("How is the final project price determined?",
      "After the free assessment, we provide a fixed-price quote based on the number of systems, complexity, compliance requirements, and target cloud environment. There are no hourly billing surprises."),
    new("Do you offer fixed-price or time-and-materials contracts?",
      "We primarily work on fixed-price engagements so you have budget certainty. For exploratory or research-heavy phases, we may use time-and-materials with a capped budget."),
    new("What if the scope changes mid-project?",
      "We use a formal change control process. Any scope changes are documented, priced, and approved before work begins. We never add costs without your explicit sign-off."),
    new("Can we start with a small pilot before committing to a full migration?",
      "Absolutely. Many clients start with a single application pilot to validate our methodology and build internal confidence before expanding to a full program."),
    new("Do you work with government and regulated industries?",
      "Yes. We have extensive experience with FedRAMP, HIPAA, PCI-DSS, and SOC 2 compliance requirements and can support public sector and regulated enterprise clients."),
  ];
}
