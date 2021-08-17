---
title: Online Hosted Instructions
permalink: index.html
layout: home
---

# Content Directory

This repository contains the hands-on lab exercises for Microsoft course [AI-102 Designing and Implementing a Microsoft Azure AI Solution](https://docs.microsoft.com/learn/certifications/courses/ai-102t00) and the equivalent [self-paced modules on Microsoft Learn](https://aka.ms/AzureLearn_AIEngineer). The exercises are designed to accompany the learning materials and enable you to practice using the technologies they describe.

To complete these exercises, you’ll require a Microsoft Azure subscription. If your instructor has not provided you with one, you can sign up for a free trial at [https://azure.microsoft.com](https://azure.microsoft.com).

## Labs

{% assign labs = site.pages | where_exp:"page", "page.url contains '/Instructions'" %}
| Module | Lab |
| --- | --- | 
{% for activity in labs  %}| {{ activity.lab.module }} | [{{ activity.lab.title }}{% if activity.lab.type %} - {{ activity.lab.type }}{% endif %}]({{ site.github.url }}{{ activity.url }}) |
{% endfor %}

